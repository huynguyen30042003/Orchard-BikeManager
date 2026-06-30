import http from 'k6/http';

export const options = {
  vus: 100,
  iterations: 100
};

export default function () {
  http.get(
    'https://localhost:5001/api/v1/weather'
  );
}
//k6 run weather-test.js