import http from 'k6/http';
import {sleep} from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        {duration: '5m', target: 100}, // below normal load
        {duration: '10m', target: 100}, //stay at 100 users for 10 minutes
        {duration: '5m', target: 0}, // scale down. Recovery stage.
    ],
    thresholds: {
        http_req_duration: ['p(99)<150'], // 99% of requests must complete below 150ms},
    }
}

export default function () {
    const url = 'http://localhost:3000/sign-up';

    const number = Math.floor(Math.random() * 9000000) + 10000000;
    const data = {
        "email": `user${number}@odin.com`,
        "password": "secret",
        "role": "user"
    };

    const headers = {'Content-Type': 'application/json'};

    http.post(url, JSON.stringify(data),
        {
            headers: headers
        });

    sleep(1);
}