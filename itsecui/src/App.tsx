import React from "react";
import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import SignIn from "./login";
import MainPage from "./MainPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" Component={SignIn}></Route>
        <Route path="/main" Component={MainPage}></Route>
      </Routes>
    </Router>
  );
}

export default App;
