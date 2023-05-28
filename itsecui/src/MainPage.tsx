import React from "react";
import SideDrawer from "./Dashboard/Drawer";
import Dashboard from "./Dashboard/Dashboard";
import { IChartData } from "./data/ChartData";

const MainPage = () => {
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
      <SideDrawer></SideDrawer>
      <Dashboard
        energyData={data}
        lightUsageData={data}
        temperatureData={data}
      ></Dashboard>
    </div>
  );
};

export default MainPage;
