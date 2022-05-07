import { CloseCircleOutlined } from "@ant-design/icons";
import { Button, Form, Input, Select } from "antd";
import React, { FC, useState } from "react";
import { useApp } from "../context";
import {
  ControlType,
  IList,
  IListResult,
  IWorkflowAction,
} from "../interfaces";

interface IWorkflowActionPage {
  id: number;
  actionTags: IWorkflowAction[];
  actionList: IWorkflowAction[];
  setActionList: React.Dispatch<React.SetStateAction<IWorkflowAction[]>>;
  onClick: (key: number) => void;
}

const WorkflowAction: FC<IWorkflowActionPage> = ({
  id,
  actionTags,
  actionList,
  setActionList,
  onClick,
}) => {
  const { get } = useApp();
  const [control, setControl] = useState<ControlType>(ControlType.Input);
  const [list, setList] = useState<IList[]>([]);

  const loadList = async (url: string) => {
    const g: Response = await get(url);

    if (g != null) {
      const d: IListResult<IList[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  };

  const getActionTuple = (key: number) => {
    const filteredAction = actionList.filter((f) => f.key !== id);
    const oldAction = actionList.find((f) => f.key === id);

    return { filteredAction, oldAction };
  };

  const onChange = (value: any) => {
    const controltype = actionTags.find((f) => f.actionFrom === value);

    if (controltype !== null && controltype !== undefined) {
      setControl(controltype.control ?? 1);

      if (controltype.link != null) {
        loadList(controltype.link);
      }

      const { filteredAction, oldAction } = getActionTuple(id);

      const newAction: IWorkflowAction = {
        ...oldAction,
        key: id,
        actionFrom: value,
      };

      setActionList([...filteredAction, newAction]);
    }
  };

  const onActionToInputChange = (value: any) => {
    onActionToChange(value.target.value);
  };

  const onActionToChange = (value: any) => {
    const { filteredAction, oldAction } = getActionTuple(id);

    const newAction: IWorkflowAction = {
      ...oldAction,
      key: id,
      actionTo: value,
    };

    setActionList([...filteredAction, newAction]);
  };

  return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-5">
      <Form.Item name={`actionfrom-${id}`}>
        <Select showSearch onChange={onChange}>
          {actionTags.map((s) => (
            <Select.Option key={s.name} value={s.actionFrom}>
              {s.name}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item name={`actionto-${id}`}>
        {control === ControlType.Input ? (
          <Input onBlur={onActionToInputChange} />
        ) : (
          <Select onChange={onActionToChange}>
            {list?.map((l) => (
              <Select.Option key={l.id} value={l.id}>
                {l.name}
              </Select.Option>
            ))}
          </Select>
        )}
      </Form.Item>
      <Form.Item>
        <Button
          type="link"
          danger
          size="small"
          onClick={() => onClick(id)}
          icon={<CloseCircleOutlined />}
        />
      </Form.Item>
    </div>
  );
};

export default WorkflowAction;
