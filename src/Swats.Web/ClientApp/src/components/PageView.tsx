import React, { FC } from "react";

interface IPageView {
  title:string
}

const PageView : FC<IPageView> = ({title, children}) => {

  return ( <div>
     <section className="flex w-full font-bold text-2xl pb-5">
        <div className="flex w-1/2">{title}</div>
        <div className="flex w-1/2 justify-end"></div>
    </section>

    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 pb-10">
      {children}
    </div>
  </div> )
}

export default PageView;