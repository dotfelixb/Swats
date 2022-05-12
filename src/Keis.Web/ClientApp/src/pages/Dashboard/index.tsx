import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IDashboardCount,
  IFetchTicket,
  IListResult,
  ISingleResult,
} from "../../interfaces";

interface IStatCard {
  count: number;
  title: string;
}

interface IDashboard {}

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
  );
};

const columns = [
  { key: "name", column: [{ title: "Subject" }, { title: "" }] },
  {
    key: "requester",
    column: [{ title: "Requester" }, { title: "Assigned To" }],
  },
  { key: "department", column: [{ title: "Department" }, { title: "Team" }] },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const Dashboard: FC<IDashboard> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [dashboardCount, setDashboardCount] = useState<IDashboardCount>();
  const [ticketList, setTicketList] = useState<IFetchTicket[]>([]);

  useEffect(() => {
    const loadCount = async () => {
      const g: Response = await get(`methods/dashboard.count`);
      const d: ISingleResult<IDashboardCount> = await g.json();

      if (g.status === 200 && d.ok) {
        setDashboardCount(d.data);
      } else {
        // TODO: display error to user
      }
    };

    const loadTicket = async () => {
      const g: Response = await get("methods/ticket.list?limit=10");

      if (g != null) {
        const d: IListResult<IFetchTicket[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setTicketList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    if (user != null && user.token) {
      loadCount();
      loadTicket();
    }
  }, [user, get]);

  return (
    <PageView title="Dashboard">
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
        <div>
          <StatCard title="My Ticket" count={dashboardCount?.myTicket ?? 0} />
        </div>
        <div>
          <StatCard
            title="My Overdue Ticket"
            count={dashboardCount?.myOverDue ?? 0}
          />
        </div>
        <div>
          <StatCard
            title="My Due today"
            count={dashboardCount?.myDueToday ?? 0}
          />
        </div>
        <div>
          <StatCard
            title="Open Ticket"
            count={dashboardCount?.openTickets ?? 0}
          />
        </div>
        <div>
          <StatCard
            title="Overdue Ticket"
            count={dashboardCount?.openTicketsDue ?? 0}
          />
        </div>
      </div>

      <div>
        <div>
          <h2 className="font-semibold">Recent Tickets</h2>
        </div>
        <DataTable showFilter={false} columns={columns}>
          {ticketList?.map((t) => (
            <tr className="px-10" key={t.id}>
              <td className="px-3 py-3">
                <Link to={`/ticket/${t.id}`}>
                  <div className="">{t.subject}</div>
                </Link>
                <div className="text-xs" style={{ color: "#9b9b9b" }}>
                  {t.code}
                </div>
              </td>

              <td className="px-3 py-3">
                <div className="">{t.requesterName}</div>
                <div className="text-xs" style={{ color: "#9b9b9b" }}>
                  {t.assignedToName}
                </div>
              </td>

              <td className="px-3 py-3">
                <div className="">{t.departmentName}</div>
                <div className="text-xs" style={{ color: "#9b9b9b" }}>
                  {t.teamName}
                </div>
              </td>

              <td className="px-3 py-3">
                <div className="">{t.createdByName}</div>
                <div className="text-xs" style={{ color: "#9b9b9b" }}>
                  {dayjs(t.createdAt).format(dateFormats.longDateFormat)}
                </div>
              </td>

              <td>
                <span className="text-gray-300"></span>
              </td>
            </tr>
          ))}
        </DataTable>
      </div>
    </PageView>
  );
};

export default Dashboard;
