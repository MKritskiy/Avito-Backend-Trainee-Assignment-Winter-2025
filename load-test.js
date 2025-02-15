import http from 'k6/http';
import { check } from 'k6';
import { Trend } from 'k6/metrics';

// Метрика для времени отклика
let responseTimeTrend = new Trend('response_time');
let iter = 0;
export let options = {
  stages: [
    { duration: '10s', target: 1000 }, // За 30 секунд увеличиваем нагрузку до 1000 пользователей
    { duration: '30s', target: 1000 },  // Держим 1000 пользователей в течение 1 минуты
    { duration: '10s', target: 0 }  // Постепенно уменьшаем нагрузку до 0
  ],
};

export default function () {
   
  // Делаем POST-запрос на /api/auth для получения токена
  let authResponse = http.post('http://localhost:8080/api/auth', JSON.stringify({
    Username: 'validUsername' + iter,
    Password: 'validPassword'
  }), {
    headers: { 'Content-Type': 'application/json' },
  });
  iter=iter+1;
  // Проверка, что токен успешно получен
  check(authResponse, {
    'auth request was successful': (r) => r.status === 200,
  });


  // Логируем время отклика
  responseTimeTrend.add(authResponse.timings.duration);
}
