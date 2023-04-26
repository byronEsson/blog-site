import "./App.css";
import {
  BrowserRouter,
  redirect,
  Route,
  Routes,
  useNavigate,
} from "react-router-dom";
import { useState } from "react";
import Login from "./components/Login";
import Home from "./components/Home";
import useToken from "./hooks/useToken";

function App() {
  const { token, setToken } = useToken();

  return (
    <BrowserRouter>
      <div className="App">
        <h1>Caitlin's Blog</h1>
        <Routes>
          <Route
            path="/login"
            element={<Login setToken={setToken} token={token} />}
          ></Route>
          <Route path="/home" element={<Home />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
