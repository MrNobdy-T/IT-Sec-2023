import React, { useEffect, useState } from "react";
import SideDrawer from "./Dashboard/Drawer";
import Dashboard from "./Dashboard/Dashboard";
import { IChartData } from "./data/ChartData";
import { AuthenticatedUser, User } from "./AuthenticatedUser";
import SignIn from "./login";
import { useNavigate } from "react-router-dom";

interface IState {
  isLoggedIn: Boolean;
  user?: User;
}

const MainPage = () => {
  const navigation = useNavigate();
  const [state, setState] = useState<IState>({
    isLoggedIn: false,
  });

  useEffect(() => {});

  const getRole = (name: string, password: string): void => {
    let user: User = { name: name, password: password, isAuthenticated: true };
    setState({
      user: user,
      isLoggedIn: true,
    });

    const url = `https://localhost:7211/api/Database/role?Username=${encodeURIComponent(
      user.name
    )}&Password=${encodeURIComponent(user.password)}`;
    fetch(url).then((x) =>
      x.text().then((y) => {
        user.role = y;
        setState({
          isLoggedIn: true,
          user: user,
        });
      })
    );
  };

  const signIn = SignIn({ func: getRole });

  const data: Array<IChartData> = [
    {
      name: "Page B ",
      uv: 3000,
      pv: 2000,
    },

    {
      name: "Page B",
      uv: 3000,
      pv: 1398,
    },
    {
      name: "Page C",
      uv: 6900,
      pv: 9800,
    },
    {
      name: "Page D",
      uv: 2780,
      pv: 3908,
    },
    {
      name: "Page E",
      uv: 1890,
      pv: 4800,
    },
    {
      name: "Page F",
      uv: 2390,
      pv: 3800,
    },
    {
      name: "Page G",
      uv: 3490,
      pv: 200,
    },
  ];

  return (
    <div>
      {!state.isLoggedIn && signIn}
      {state.isLoggedIn && (
        <div>
          <SideDrawer user={state.user}></SideDrawer>
          <Dashboard
            energyData={data}
            lightUsageData={data}
            temperatureData={data}
          ></Dashboard>
        </div>
      )}
    </div>
  );
};

export default MainPage;
