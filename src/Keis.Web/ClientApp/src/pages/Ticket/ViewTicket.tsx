import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTicket, ISingleResult } from "../../interfaces";

interface IViewTicket {}

const ViewTicket: FC<IViewTicket> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [ticket, setTicket] = useState<IFetchTicket>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/ticket.get?id=${id}`);
      const d: ISingleResult<IFetchTicket> = await g.json();

      if (g.status === 200 && d.ok) {
        setTicket(d.data);
      } else {
        // TODO: display error to user
      }
    };

    if (user != null && user.token && id) {
      load();
    }
  }, [user, id, get]);

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/ticket">Tickets</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Ticket</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={ticket?.code ?? "View Ticket"}
      breadcrumbs={<Breadcrumbs />}
    >
      <div className="w-full flex flex-row ">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{ticket?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(ticket?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{ticket?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(ticket?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10"></div>
      </div>
    </PageView>
  );
};

export default ViewTicket;
