import { useState } from "react";
import { authorizeLogin } from "../api";

function Login({ setToken }) {
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const handleSubmit = async (e) => {
    e.preventDefault();
    const { token } = await authorizeLogin({ email, password });

    setToken((t) => token);
  };
  return (
    <form onSubmit={handleSubmit}>
      <label>Email:</label>
      <input name="email" onChange={(e) => setEmail(e.target.value)}></input>
      <label>Password</label>
      <input
        name="password"
        onChange={(e) => setPassword(e.target.value)}
      ></input>
      <button type="submit">Submit</button>
    </form>
  );
}

export default Login;
