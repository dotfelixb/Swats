import { Breadcrumb } from 'antd';
import React, { FC, useCallback, useEffect, useState } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { PageView } from '../../components';
import { useApp, useAuth } from '../../context';
import {  IListResult, ISingleResult } from '../../interfaces';
import TemplateForm from './TemplateForm';

interface IFormData {
  name: string;
  status: string;
  subject: string;
  mergetags: string[];
  body: string;
}

interface INewTemplate {

}

const NewTemplate: FC<INewTemplate> = () => {
  const { user } = useAuth();
  const { get, post } = useApp();
  const navigate = useNavigate();
  const [mergeList, setMergeList] = useState<string[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const loadTemplateMergeTags = useCallback(async () => {
    const g: Response = await get(`methods/template.mergetags`);

    if (g != null) {
      const d: IListResult<string[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setMergeList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null && user.token) {
      loadTemplateMergeTags();
    }
  }, [user, get, loadTemplateMergeTags]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("name", values.name ?? "");
    body.append("subject", values.subject ?? "");
    body.append("status", values.status ?? "");
    body.append("body", values.body ?? "");

    for (let index = 0; index < values.mergetags.length; index++) {
      body.append(`mergetags[${index}]`, values.mergetags[index]);
    }

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await post("methods/template.create",  body );

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/template/${result.data}`, { replace: true });
    } else {
      setHasFormErrors(true);
      setFormErrors(result?.errors);
    }
  };

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/template">Template</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Template</Breadcrumb.Item>
    </Breadcrumb>
  );
  return ( <PageView title="New Template" breadcrumbs={<Breadcrumbs />}>
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <TemplateForm
            hasFormErrors={hasFormErrors}
            formErrors={formErrors}
            mergeList={mergeList}
            onFinish={onFinish}
          />
        </div>

        <div></div>
      </div>

      <Outlet />
  </PageView>);
}

export default NewTemplate;