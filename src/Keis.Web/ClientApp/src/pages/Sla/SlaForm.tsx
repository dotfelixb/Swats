import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
  ScheduleOutlined,
} from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchBusinessHour, IFetchSla } from "../../interfaces";

const { TextArea } = Input;

interface ISlaForm {
  hasFormErrors: boolean;
  formErrors: string[];
  sla?: IFetchSla;
  hourList: IFetchBusinessHour[];
  onFinish: (values: any) => void;
}

const SlaForm: FC<ISlaForm> = ({
  hasFormErrors,
  formErrors,
  sla,
  hourList,
  onFinish,
}) => {
  return (
    <Form layout="vertical" onFinish={onFinish}>
      {hasFormErrors &&
        formErrors?.map((e) => (
          <div key={e} className="py-2">
            <Alert message={e} type="error" className="py-2" />
          </div>
        ))}
      <Timeline>
        <Timeline.Item dot={<FormOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">Sla info</div>
          <Form.Item name="name" label="Name" initialValue={sla?.name}>
            <Input />
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item
          dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
        >
          <div className="font-bold mb-2">Working hours and status</div>
          <Form.Item
            name="hour"
            label="Business Hour"
            initialValue={sla?.businessHour}
          >
            <Select showSearch>
              {hourList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="status" label="Status" initialValue={sla?.status}>
            <Select>
              <Select.Option value="1">Active</Select.Option>
              <Select.Option value="2">Inactive</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<ScheduleOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">
            Sla actions and resolution period
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-5">
            <Form.Item
              name="responseperiod"
              label="Response Period"
              initialValue={sla?.responsePeriod}
            >
              <Input type="number" />
            </Form.Item>
            <Form.Item
              name="responseformat"
              label="Period Format"
              initialValue={sla?.responseFormat}
            >
              <Select>
                <Select.Option value="1">Minute(s)</Select.Option>
                <Select.Option value="2">Hour(s)</Select.Option>
                <Select.Option value="3">Day(s)</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item
              name="responsenotify"
              label="Inapp Notification"
              initialValue={sla?.responseNotify}
            >
              <Select>
                <Select.Option value={true}>Yes</Select.Option>
                <Select.Option value={false}>No</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item
              name="responseemail"
              label="Escalation Email"
              initialValue={sla?.responseEmail}
            >
              <Select>
                <Select.Option value={true}>Yes</Select.Option>
                <Select.Option value={false}>No</Select.Option>
              </Select>
            </Form.Item>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-5">
            <Form.Item
              name="resolveperiod"
              label="Resolution Period"
              initialValue={sla?.resolvePeriod}
            >
              <Input type="number" />
            </Form.Item>
            <Form.Item
              name="resolveformat"
              label="Period Format"
              initialValue={sla?.resolveFormat}
            >
              <Select>
                <Select.Option value="1">Minute(s)</Select.Option>
                <Select.Option value="2">Hour(s)</Select.Option>
                <Select.Option value="3">Day(s)</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item
              name="resolvenotify"
              label="Inapp Notification"
              initialValue={sla?.resolveNotify}
            >
              <Select>
                <Select.Option value={true}>Yes</Select.Option>
                <Select.Option value={false}>No</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item
              name="resolveemail"
              label="Escalation Email"
              initialValue={sla?.resolveEmail}
            >
              <Select>
                <Select.Option value={true}>Yes</Select.Option>
                <Select.Option value={false}>No</Select.Option>
              </Select>
            </Form.Item>
          </div>
        </Timeline.Item>
        <Timeline.Item dot={<CommentOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">
            Addition description for this SLA (optional)
          </div>
          <Form.Item name="note" label="Note" initialValue={sla?.note}>
            <TextArea rows={4} />
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit">
              Submit
            </Button>
          </Form.Item>
        </Timeline.Item>
      </Timeline>
    </Form>
  );
};

export default SlaForm;
