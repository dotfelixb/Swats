import React, { FC } from "react";

interface INoMatch {}

const NoMatch : FC<INoMatch> = () => {

  return ( <div>
    <span>No Match</span> 
  </div> )
}

export default NoMatch;