import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchBusinessHour, IListResult } from "../../interfaces";

interface IListHours {}

interface IHourListItem {
  data: IFetchBusinessHour;
}

const HourListItem = ({ data }: IHourListItem) => {
  const {dateFormats} = useApp()

  return (
    <tr className="px-10" key={data.id}>
      <td className="px-3 py-1">
        <Link to={`/admin/businesshour/${data.id}`}>
          <div className="">{data.name}</div>
        </Link>
        <div className="text-xs" style={{ color: "#9b9b9b" }}>
          {data.description}
        </div>
      </td>

      <td className="px-3 py-3">
        <div className="">{data.status}</div>
        <div className="text-xs" style={{ color: "#9b9b9b" }}>
          {data.timezone}
        </div>
      </td>
      <td className="px-3 py-3">
        <div className="">{data.createdByName}</div>
        <div className="text-xs" style={{ color: "#9b9b9b" }}>
          {dayjs(data.createdAt).format(dateFormats.longDateFormat)}
        </div>
      </td>
      <td>
        <span className="text-gray-300"></span>
      </td>
    </tr>
  );
};

const ListHours: FC<IListHours> = () => {
  const { user } = useAuth();
  const { get } = useApp();
  const [hourList, setHourList] = useState<IFetchBusinessHour[]>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get("methods/businesshour.list");
      const d: IListResult<IFetchBusinessHour[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setHourList(d.data);
      } else {
        // TODO: display error to user
      }
    };

    // because useEffect is not async
    if (user != null && user.token) {
      load();
    }
  }, [user, get]);

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="new">
        <Button type="primary">New Hour</Button>
      </Link>
    </div>
  );

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>Business Hours</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Business Hours"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <div className="bg-white border border-gray-200 rounded-sm">
        {/* filter fields */}
        <div className="grid grid-cols-5 gap-4 px-5 py-5">
          <div>
            <div className="w-full">
              <input className="form-input-search" placeholder="Name" />
            </div>
          </div>
          <div>
            <div className="w-full">
              <input className="form-input-search" placeholder="Name" />
            </div>
          </div>
        </div>

        {/* filter tags */}
        <div className="w-full px-5 pb-3">
          <span className="text-xs rounded bg-indigo-500 text-white px-2 py-1">
            Filter
          </span>
          <span className="text-xs rounded bg-indigo-500 text-white px-2 py-1">
            Name
          </span>
        </div>

        {/* table */}
        <div className="w-full border-t-2 border-gray-50">
          <table className="w-full table-auto">
            <thead>
              <tr className="text-left px-3 bg-indigo-50">
                <th className="px-3 py-3 font-semibold">
                  <div>Name</div>
                  <div className="text-xs" style={{ color: "#6b6b6b" }}>
                    Description
                  </div>
                </th>
                <th className="px-3 py-3 font-semibold ">
                  <div>Status</div>
                  <div className="text-xs" style={{ color: "#6b6b6b" }}>
                    Timezone
                  </div>
                </th>
                <th className="px-3 py-3 font-semibold">
                  <div>Created By</div>
                  <div className="text-xs" style={{ color: "#6b6b6b" }}>
                    Created At
                  </div>
                </th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {hourList?.map((h) => (
                <HourListItem key={h.id} data={h} />
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <Outlet />
    </PageView>
  );
};

export default ListHours;
