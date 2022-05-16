import { Button, Form, Modal, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchTopic } from "../../../interfaces";

interface IChangeTopic {
  showModal: boolean;
  topicList: IFetchTopic[];
  onHideModal: (value: React.SetStateAction<boolean>) => void;
  onSubmit: (values: any) => void;
}

const ChangeTopic: FC<IChangeTopic> = ({showModal, topicList, onHideModal, onSubmit }) => (<Modal
  visible={showModal}
  title="Change ticket assign Help Topic"
  onCancel={() => onHideModal(false)}
  destroyOnClose={true}
  footer={null}
>
  <Form layout="vertical" onFinish={onSubmit}>
    <Timeline>
      <Timeline.Item>
        <div className="font-bold mb-2">Who should work on it</div>
        <Form.Item name="helpTopic" label="Help Topic">
          <Select showSearch>
            {topicList?.map((a) => (
              <Select.Option key={a.id} value={a.id}>
                {a.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
      </Timeline.Item>
      <Timeline.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Update
          </Button>
        </Form.Item>
      </Timeline.Item>
    </Timeline>
  </Form>
</Modal>)

export default ChangeTopic;