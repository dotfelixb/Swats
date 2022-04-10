import {
  AimOutlined,
  ApartmentOutlined,
  CalendarOutlined,
  CheckCircleOutlined,
  ClusterOutlined,
  FormatPainterOutlined,
  HourglassOutlined,
  ImportOutlined,
  InfoCircleOutlined,
  InteractionOutlined,
  MailOutlined,
  NotificationOutlined,
  PhoneOutlined,
  PrinterOutlined,
  StarOutlined,
  TagsOutlined,
  TeamOutlined,
  UserOutlined,
  UserSwitchOutlined,
} from "@ant-design/icons";
import { Breadcrumb } from "antd";
import React, { FC } from "react";
import { Link } from "react-router-dom";
import { PageView } from "../../components";

interface ILinkCard {
  title: string;
  location: string;
  icon: React.ReactNode;
}

const LinkCard: FC<ILinkCard> = ({ title, location, icon }) => {
  return (
    <div className="w-full flex flex-col justify-center items-center min-w-fit bg-white border border-gray-200 rounded px-3 py-10">
      <Link to={location} className="cursor-pointer">
        <div className="relative flex justify-center border-solid border-4 border-indigo-200 rounded-full w-16 h-16">
          <span className=" text-lg font-bold text-center text-indigo-500 mt-2">
            {icon}
          </span>
        </div>
      </Link>
      <div className="flex justify-center items-center pt-5">
        <span className="text-sm text-center">{title}</span>
      </div>
    </div>
  );
};

interface ISettings {}

const Settings: FC<ISettings> = () => {
  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>Settings</Breadcrumb.Item>
    </Breadcrumb>
  );
  
  return (
    <PageView title="Admin Panel" breadcrumbs={<Breadcrumbs />}>
      <div className="">
        <section className="title font-semibold text-sm py-5">Staff</section>

        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
          <div>
            <LinkCard
              title="Agents"
              location="/admin/agent"
              icon={<PhoneOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Departments"
              location="/admin/department"
              icon={<UserSwitchOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Teams"
              location="/admin/team"
              icon={<TeamOutlined />}
            />
          </div>
        </div>

        <section className="title font-semibold text-sm py-5">Email</section>

        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
          <div>
            <LinkCard
              title="Email Settings"
              location="/admin/email"
              icon={<MailOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Breaklines"
              location="/admin/breaklines"
              icon={<ClusterOutlined />}
            />
          </div>
        </div>

        <section className="title font-semibold text-sm py-5">Manage</section>

        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
          <div>
            <LinkCard
              title="Help Topic"
              location="/admin/helptopic"
              icon={<InfoCircleOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Business Hour"
              location="/admin/businesshour"
              icon={<CalendarOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Sla Plan"
              location="/admin/sla"
              icon={<HourglassOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Workflow"
              location="/admin/workflow"
              icon={<ApartmentOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Approval Workflow"
              location="/admin/approval"
              icon={<CheckCircleOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Tags"
              location="/admin/tag"
              icon={<TagsOutlined />}
            />
          </div>
        </div>

        <section className="title font-semibold text-sm py-5">Tickets</section>

        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
          <div>
            <LinkCard
              title="Ticket Types"
              location="/admin/tickettype"
              icon={<AimOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Ratings"
              location="/admin/ratings"
              icon={<StarOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Recurring"
              location="/admin/recurring"
              icon={<InteractionOutlined />}
            />
          </div>
        </div>

        <section className="title font-semibold text-sm py-5">Settings</section>

        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
          <div>
            <LinkCard
              title="User Imports"
              location="/admin/userimport"
              icon={<ImportOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="User Options"
              location="/admin/useroptions"
              icon={<UserOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Logs"
              location="/admin/logs"
              icon={<PrinterOutlined />}
            />
          </div>
        </div>

        <section className="title font-semibold text-sm py-5">
          Notifications
        </section>

        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
          <div>
            <LinkCard
              title="Notifications"
              location="/admin/notification"
              icon={<NotificationOutlined />}
            />
          </div>
          <div>
            <LinkCard
              title="Templates"
              location="/admin/template"
              icon={<FormatPainterOutlined />}
            />
          </div>
        </div>
      </div>
    </PageView>
  );
};

export default Settings;
