import http from 'k6/http';


export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 10,
    duration: '10s'
};

export default function () {
    const url = 'http://localhost:3000/sign-up';

    const number = Math.floor(Math.random() * 90000) + 10000;
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
}