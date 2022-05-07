import React, { FC, useEffect } from "react";

interface IPageView {
  title: string;
  buttons?: React.ReactNode;
  breadcrumbs?: React.ReactNode;
}

const PageView: FC<IPageView> = ({ title, children, buttons, breadcrumbs }) => {
  useEffect(() => {
    document.title = title;
  }, [title]);

  return (
    <div>
      <section>
        <div className="flex flex-row text-xs text-indigo-400 py-3 items-center">
          {breadcrumbs && breadcrumbs}
        </div>
      </section>
      <section className="flex w-full items-center pb-5">
        <div className="flex w-1/2 font-bold text-2xl ">{title}</div>
        <div className="flex w-1/2 items-center justify-end">
          {buttons && buttons}
        </div>
      </section>

      <div>{children}</div>
    </div>
  );
};

export default PageView;
