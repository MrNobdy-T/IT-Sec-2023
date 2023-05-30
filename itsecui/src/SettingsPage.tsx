import React, { useEffect, useState } from "react";
import SideDrawer from "./Dashboard/Drawer";
import { User } from "./AuthenticatedUser";
import { useLocation } from "react-router-dom";

const SettingsPage = () => {
  const [isOn, setIsOn] = useState(false);
  const location = useLocation();
  let user;

  const [targetTemperature, setTargetTemperature] = useState(25);

  useEffect(() => {});

  const handleToggle = () => {
    setIsOn(!isOn);
  };

  const handleTemperatureChange = (e: any) => {
    setTargetTemperature(e.target.value);
  };

  return (
    <div>
      <SideDrawer
        user={{
          name: "admin123",
          password: "password",
          isAuthenticated: true,
          role: "admin",
        }}
      ></SideDrawer>
      <h2>Settings</h2>
      <div>
        <label htmlFor="toggle">Toggle Smart Home: </label>
        <input
          id="toggle"
          type="checkbox"
          checked={isOn}
          onChange={handleToggle}
        />
      </div>
      <div>
        <label htmlFor="targetTemperature">Target Temperature: </label>
        <input
          id="targetTemperature"
          type="number"
          value={targetTemperature}
          onChange={handleTemperatureChange}
        />
      </div>
    </div>
  );
};

export default SettingsPage;
