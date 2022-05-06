import {CloseCircleOutlined} from "@ant-design/icons";
import {Button, Form, Input, Select} from "antd";
import React, {FC, useState} from "react";
import {useApp} from "../context";
import {ControlType, IList, IListResult, IWorkflowCriteria} from "../interfaces";

interface IWorkflowCriteriaPage {
    id: number;
    criteriaTags: IWorkflowCriteria[];
    criteriaList: IWorkflowCriteria[];
    setCriteriaList: React.Dispatch<React.SetStateAction<IWorkflowCriteria[]>>;
    onClick: (key: number) => void;
}

const WorkflowCriteria: FC<IWorkflowCriteriaPage> = ({
                                                         id,
                                                         criteriaTags,
                                                         criteriaList,
                                                         setCriteriaList,
                                                         onClick,
                                                     }) => {
    const {get} = useApp();
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

    const getCriteriaTuple = (key: number) => {
        const filteredCriteria = criteriaList.filter(f => f.key !== id);
        const oldCriteria = criteriaList.find(f => f.key === id);

        return {filteredCriteria, oldCriteria}
    }

    const onChange = (value: any) => {
        const controltype = criteriaTags.find((f) => f.name === value);

        if (controltype !== null && controltype !== undefined) {
            setControl(controltype.control ?? 1);

            if (controltype.link != null) {
                loadList(controltype.link);
            }

            const {filteredCriteria, oldCriteria} = getCriteriaTuple(id);

            const newCriteria: IWorkflowCriteria = {
                ...oldCriteria,
                key: id,
                criteria: value
            };

            setCriteriaList([...filteredCriteria, newCriteria]);
        }
    };

    const onMatchChange = (value: any) => {
        const {filteredCriteria, oldCriteria} = getCriteriaTuple(id);

        const newCriteria: IWorkflowCriteria = {
            ...oldCriteria,
            key: id,
            condition: value
        };

        setCriteriaList([...filteredCriteria, newCriteria]);
    }

    const onThisInputChange = (value: any) => {
        onThisChange(value.target.value);
    }

    const onThisChange = (value: any) => {
        const {filteredCriteria, oldCriteria} = getCriteriaTuple(id);

        const newCriteria: IWorkflowCriteria = {
            ...oldCriteria,
            key: id,
            match: value
        };

        setCriteriaList([...filteredCriteria, newCriteria]);
    }

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
            <div className="grid grid-cols-2 gap-5">
                <Form.Item name={`when-${id}`}>
                    <Select showSearch onChange={onChange}>
                        {criteriaTags.map((s) => (
                            <Select.Option key={s.name} value={s.name}>
                                {s.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>
                <Form.Item name={`match-${id}`}>
                    <Select onChange={onMatchChange}>
                        <Select.Option value="1">Equals</Select.Option>
                        <Select.Option value="2">Contains</Select.Option>
                    </Select>
                </Form.Item>
            </div>
            <Form.Item name={`this-${id}`}>
                <div className="flex flex-row items-center">
                    {control === ControlType.Input ? (
                        <Input onBlur={onThisInputChange}/>
                    ) : (
                        <Select onChange={onThisChange}>
                            {list?.map((l) => (
                                <Select.Option key={l.id} value={l.id}>
                                    {l.name}
                                </Select.Option>
                            ))}
                        </Select>
                    )}
                    <Button
                        type="link"
                        danger
                        size="small"
                        className="ml-3"
                        onClick={() => onClick(id)}
                        icon={<CloseCircleOutlined/>}
                    />
                </div>
            </Form.Item>
        </div>
    );
};

export default WorkflowCriteria;
