import { SaveOutlined } from "@ant-design/icons";
import { Breadcrumb, Button, Form, Input, Select } from "antd";
import { FC } from "react";
import { Link, Outlet } from "react-router-dom";
import { PageView } from "../../components";

interface INewTicket { }

const NewTicket: FC<INewTicket> = () => {
  const [form] = Form.useForm();

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Button type="primary"  >
        <div className="flex flex-row items-center space-x-2">
          <SaveOutlined />
          <div>Save</div>
        </div>
      </Button>
    </div>
  );

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item href="">
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item href="">
        <Link to="/ticket">Tickets</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Ticket</Breadcrumb.Item>
    </Breadcrumb>
  )

  return (<PageView title="New Ticket" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>

    <Form form={form} layout="vertical" >
      <div className="w-full rounded-sm px-10 py-10 border-gray-200 border">

        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <Form.Item label="Requester" >
            <Select showSearch>
              <Select.Option value="1">Dzifa</Select.Option>
              <Select.Option value="2">Ruth</Select.Option>
              <Select.Option value="3">Lucy</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item label="Subject"  >
            <Input />
          </Form.Item>
        </div>

      </div>
    </Form>

    <Outlet />
  </PageView>)
}

export default NewTicket;