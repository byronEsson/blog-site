import "./App.css";
import { BrowserRouter } from "react-router-dom";
import { useState } from "react";
import Login from "./components/Login";

function App() {
  const [token, setToken] = useState();

  return (
    <BrowserRouter>
      <div className="App">
        <h1>Caitlin's Blog</h1>
        {!token ? <Login setToken={setToken}></Login> : <p>{token}</p>}
      </div>
    </BrowserRouter>
  );
}

export default App;
