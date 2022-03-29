import {
  BookOutlined,
  CommentOutlined,
  SaveOutlined,
  SkinOutlined,
} from "@ant-design/icons";
import { Breadcrumb, Button, Form, Input, Select, Timeline } from "antd";
import { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { PageView } from "../../components";
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';

// const { TextArea } = Input;

interface INewTicket {}

const NewTicket: FC<INewTicket> = () => {
  const [form] = Form.useForm();
  const [value, setValue] = useState('');

  useEffect(()=> {
    console.log(value);
    
  }, [value])

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Button type="primary">
        <div className="flex flex-row items-center space-x-2">
          <SaveOutlined />
          <div>Submit</div>
        </div>
      </Button>
    </div>
  );

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
      title="Create ticket"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <Form form={form} layout="vertical">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
          <div>
            <Timeline>
              <Timeline.Item
                dot={<BookOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Where is the ticket from</div>
                <Form.Item label="Requester" htmlFor="requester">
                  <Select showSearch id="requester">
                    <Select.Option value="1">Dzifa</Select.Option>
                    <Select.Option value="2">Ruth</Select.Option>
                    <Select.Option value="3">Lucy</Select.Option>
                  </Select>
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<SkinOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Who should work on it</div>
                <Form.Item label="Assigned To">
                  <Select showSearch>
                    <Select.Option value="1">Dzifa</Select.Option>
                    <Select.Option value="2">Ruth</Select.Option>
                    <Select.Option value="3">Lucy</Select.Option>
                  </Select>
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">What is the issue</div>
                <Form.Item label="Subject">
                  <Input />
                </Form.Item>
                <Form.Item label="Description">
                  {/* <TextArea rows={4} /> */}
                  <ReactQuill theme="snow" value={value} onChange={setValue} style={{height: '240px'}}/>
                </Form.Item>
              </Timeline.Item>
            </Timeline>
          </div>

          <div></div>
        </div>
      </Form>
      <Outlet />
    </PageView>
  );
};

export default NewTicket;
