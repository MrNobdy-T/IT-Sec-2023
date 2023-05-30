import React from "react";
import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import SignIn from "./login";
import MainPage from "./MainPage";
import SettingsPage from "./SettingsPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/admin" Component={SettingsPage}></Route>
        <Route path="/" Component={MainPage}></Route>
      </Routes>
    </Router>
  );
}

export default App;
