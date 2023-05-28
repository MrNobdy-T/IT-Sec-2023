import React, { ReactElement } from "react";
import { CartesianGrid, Line, LineChart, XAxis, YAxis } from "recharts";
import { IChartData } from "../data/ChartData";

interface GenericChartProps {
  width: number;
  height: number;
  data: Array<IChartData>;
}

export const GenericChart = (props: GenericChartProps): ReactElement => {
  return (
    <div>
      <LineChart width={props.width} height={props.height} data={props.data}>
        <XAxis dataKey="name" />
        <YAxis />
        <CartesianGrid stroke="#eee" strokeDasharray="5 5" />
        <Line type="monotone" dataKey="uv" stroke="#8884d8" />
        <Line type="monotone" dataKey="pv" stroke="#82ca9d" />
      </LineChart>
    </div>
  );
};
