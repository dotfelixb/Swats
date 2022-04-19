import React, { FC } from "react";
import { PageView } from "../../components";

interface IStatCard {
    count: number;
    title: string;
}

interface IDashboard { }

const StatCard: FC<IStatCard> = ({ title, count }) => {
    return (
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

const Dashboard: FC<IDashboard> = () => {
    return (<PageView title="Dashboard">
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">

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
        </div>
    </PageView>)
}

export default Dashboard;