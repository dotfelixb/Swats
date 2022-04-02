import { CommentOutlined, EyeOutlined, FormOutlined, SaveOutlined } from '@ant-design/icons';
import { Breadcrumb, Button, Form, Select, Input, Timeline } from 'antd';
import React, { FC } from 'react';
import { Link, Outlet } from 'react-router-dom';
import { PageView } from '../../components';

const {TextArea} = Input;

interface INewType {}

const NewType : FC<INewType> = () => {
  const [form] = Form.useForm();

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
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/tickettype">Ticket Types</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Type</Breadcrumb.Item>
    </Breadcrumb>
  );
  
  return (<PageView title='New Type' buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>
    <Form form={form} layout="vertical">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
          <div>
            <Timeline>
              <Timeline.Item
                dot={<FormOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Ticket type name</div>
                <Form.Item label="Name" htmlFor="name">
                  <Input name='name' />
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<EyeOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">External visibility and status</div>
                <Form.Item label="Visibility">
                  <Select >
                    <Select.Option value="1">Public</Select.Option>
                    <Select.Option value="2">Private</Select.Option>
                  </Select>
                </Form.Item>
                <Form.Item label="Status">
                  <Select >
                    <Select.Option value="1">Active</Select.Option>
                    <Select.Option value="2">Inactive</Select.Option>
                  </Select>
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">The description of this type (optional)</div>
                <Form.Item label="Description" htmlFor='description'>
                  <TextArea name='description' rows={4} />
                 </Form.Item>
              </Timeline.Item>
            </Timeline>
          </div>

          <div></div>
        </div>
      </Form>
      <Outlet />
  </PageView> )
}

export default NewType;