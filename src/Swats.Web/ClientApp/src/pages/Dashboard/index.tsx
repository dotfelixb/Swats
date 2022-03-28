import React, { FC } from "react";
import { PageView } from "../../components";

interface IStatCard {
  count: number;
  title:string;
}

interface IDashboard {}

const StatCard : FC<IStatCard> = ({title, count}) => {
  return(
    <div>
     <div className="statcard-container">
        <div className="statcard-ring">
            <span className="">{count}</span>
            <div className=""></div>
        </div>
        <div className="statcard-title">
            <span className="">{title}</span>
        </div>
    </div>
</div>
  )
}

const Dashboard : FC<IDashboard> = () => {

  return ( <PageView title="Dashboard">
    <div>
      <StatCard title="My Ticket" count={6} />
    </div>
    <div>
      <StatCard title="My Overdue Ticket" count={3} />
    </div>
    <div>
      <StatCard title="My Due today" count={3} />
    </div>
    <div>
      <StatCard title="Open Ticket" count={23} />
    </div>
    <div>
      <StatCard title="Overdue Ticket" count={4} />
    </div>
  </PageView> )
}

export default Dashboard;