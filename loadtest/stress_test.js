import http from 'k6/http';
import {sleep} from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        {duration: '2m', target: 100}, // below normal load
        {duration: '5m', target: 100},
        {duration: '2m', target: 200}, // normal load
        {duration: '5m', target: 200},
        {duration: '2m', target: 300}, // around the breaking point
        {duration: '5m', target: 300},
        {duration: '2m', target: 400}, // beyond the breaking point
        {duration: '5m', target: 400},
        {duration: '10m', target: 0}, // scale down. Recovery stage.
    ]
}

export default function () {
    const url = 'http://localhost:3000/sign-up';

    const number = Math.floor(Math.random() * 90000) + 10000000;
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