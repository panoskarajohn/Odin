import http from 'k6/http';
import {sleep} from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        {duration: '10s', target: 100}, // below normal load
        {duration: '1m', target: 100},
        {duration: '10s', target: 1000}, // spike to 1000 users
        {duration: '3m', target: 1000},
        {duration: '10s', target: 100}, // scale down. Recovery stage.
        {duration: '3m', target: 100},
        {duration: '10s', target: 0}, // scale down. Recovery stage.
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