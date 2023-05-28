import React from "react";
import { GenericChart } from "../Charts/GenericChart";
import { IChartData } from "../data/ChartData";
import { Box } from "@mui/material";

interface DashboardParams {
  temperatureData: Array<IChartData>;
  energyData: Array<IChartData>;
  lightUsageData: Array<IChartData>;
}

const Dashboard = (param: DashboardParams) => {
  const temperatureData = param.temperatureData;
  const energyConsumptionData = param.energyData;
  const lightUsageData = param.lightUsageData;

  return (
    <div>
      <Box sx={{ display: "inline-flex", margin: "10%" }}>
        <GenericChart
          data={temperatureData}
          width={350}
          height={250}
        ></GenericChart>
        <GenericChart
          data={energyConsumptionData}
          width={350}
          height={250}
        ></GenericChart>
        <GenericChart
          data={lightUsageData}
          width={350}
          height={250}
        ></GenericChart>
      </Box>
    </div>
  );
};

export default Dashboard;
