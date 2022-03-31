import React, { FC } from "react";
import { Link } from "react-router-dom";

interface ILinkCard {
  title: string;
  location: string;
}

const LinkCard : FC<ILinkCard> = ({title, location}) => {
  return (
    <div className="w-full flex flex-col justify-center items-center min-w-fit bg-white border border-gray-200 rounded px-3 py-10">
    <Link to={location} className="cursor-pointer" >
        <div className="relative flex justify-center items-center border-solid border-4 border-indigo-200 rounded-full w-16 h-16">
            <span className="text-lg font-bold text-center text-indigo-500 mt-2">
                {/* icon */}
            </span>
        </div>
    </Link>
    <div className="flex justify-center items-center pt-5">
        <span className="text-xs text-center">{title}</span>
    </div>
</div>)
}

interface ISettings {}

const Settings : FC<ISettings> = () => {

  return ( <div className="">
    <section className="title font-semibold text-sm py-5">
        Staff
    </section>

    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
      <div>
        <LinkCard title="Agents" location="/admin/agent" />
      </div>
      <div>
        <LinkCard title="Departments" location="/admin/department" />
      </div>
      <div>
        <LinkCard title="Teams" location="/admin/team" />
      </div>
    </div>
  </div> )
}

export default Settings;