import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7132/api/",
});

export const authorizeLogin = (body) => {
  console.log(body);
  return api.post(`Auth/login`, body).then(({ data }) => {
    return data;
  });
};
