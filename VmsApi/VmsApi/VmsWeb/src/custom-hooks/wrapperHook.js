import React, { Component } from "react";
import useToken from "./useToken";

export const withHooksComponent = (Component) => {
  return (props) => {
    const { token, userDetail, logout } = useToken();

    return (
      <Component
        token={token}
        logout={logout}
        userDetail={userDetail}
        {...props}
      />
    );
  };
};
