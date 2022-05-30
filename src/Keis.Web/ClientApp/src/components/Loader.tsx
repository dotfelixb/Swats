import { LoadingOutlined } from "@ant-design/icons";
import React, { FC } from "react";

interface ILoader {}

const Loader: FC<ILoader> = () => {
  return (
    <div className="w-full h-screen flex flex-col justify-center items-center ">
      <div>
        <LoadingOutlined className="text-4xl font-bold" />
      </div>
      <div className="py-2 font-bold">Responding to the World ☏</div>
    </div>
  );
};

export default Loader;
