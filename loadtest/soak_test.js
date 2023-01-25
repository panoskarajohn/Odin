import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '2m',  target: 400 },
        { duration: '3h56m',  target: 400 },
        { duration: '2m',  target: 0 },
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

    const headers = { 'Content-Type': 'application/json' };

    http.post(url, JSON.stringify(data),
    {
        headers: headers
    });

    sleep(1);
}