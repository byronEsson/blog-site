import { useEffect, useState } from "react";
import { authorizeLogin } from "../api";
import { redirect, useNavigate } from "react-router-dom";

function Login({ setToken, token }) {
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const handleSubmit = async (e) => {
    e.preventDefault();
    const tokenObject = await authorizeLogin({ email, password });

    setToken(tokenObject);
  };

  useEffect(() => {
    if (token) navigate("/home");
  });
  const navigate = useNavigate();
  return (
    <form onSubmit={handleSubmit}>
      <label>Email:</label>
      <input name="email" onChange={(e) => setEmail(e.target.value)}></input>
      <label>Password</label>
      <input
        type="password"
        name="password"
        onChange={(e) => setPassword(e.target.value)}
      ></input>
      <button type="submit">Submit</button>
    </form>
  );
}

export default Login;
