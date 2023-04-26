import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7132/api/",
});

export const authorizeLogin = async (body) => {
  console.log(body);
  const { data } = await api.post(`Auth/login`, body);
  return data;
};
