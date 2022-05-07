import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTicket, IListResult } from "../../interfaces";

interface IListTickets {}

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

const ListTickets: FC<IListTickets> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [ticketList, setTicketList] = useState<IFetchTicket[]>([]);

  useEffect(() => {
    const load = async () => {
      console.log("load!");

      const g: Response = await get("methods/ticket.list");

      if (g != null) {
        const d: IListResult<IFetchTicket[]> = await g.json();

        if (g.status === 200 && d.ok) {
          console.log("here!");

          setTicketList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    if (user != null && user.token) {
      load();
    }
  }, [user, get]);

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="/ticket/new">
        <Button type="primary">New Ticket</Button>
      </Link>
    </div>
  );

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>Tickets</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Tickets"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <DataTable columns={columns}>
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

      <Outlet />
    </PageView>
  );
};

export default ListTickets;
