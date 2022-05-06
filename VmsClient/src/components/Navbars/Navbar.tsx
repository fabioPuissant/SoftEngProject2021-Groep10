import React from "react";
import classNames from "classnames";
// import PropTypes from 'prop-types';
// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import IconButton from "@material-ui/core/IconButton";
import Hidden from "@material-ui/core/Hidden";
// @material-ui/icons
import Menu from "@material-ui/icons/Menu";
// core components
import AdminNavbarLinks from "./AdminNavbarLinks";
import Button from "../CustomButtons/Button";

import headerStyle from "../../assets/jss/material-dashboard-react/components/headerStyle";
import { Link, Redirect } from "react-router-dom";
import { checkRole } from "../../utils/RoleChecker";
import { withHooksComponent } from "../../custom-hooks/wrapperHook";

function Header({ ...props }: any) {
  const { classes, color } = props;
  const appBarClasses = classNames({
    [" " + classes[color]]: color,
  });

  const agendaLink = 
    <Button color="transparent" href="#" className={classes.title}>
      <Link to={"/agenda/overview"} className={classes.title}>
        Agenda
      </Link>
    </Button>

  const dashboardLink = checkRole(
    props.userDetail.roles != undefined ? props.userDetail.roles : [],
    ["Weger", "Voedingsbeheerder", "Manager", "Administrator"]
  ) ? (
    <Button color="transparent" href="#" className={classes.title}>
      <Link to={"/dashboard/overview"} className={classes.title}>
        Dashboard
      </Link>
    </Button>
  ) : null;

  // if (!checkRole(
  //   props.userDetail.roles != undefined ? props.userDetail.roles : [],
  //   ["Weger", "Voedingsbeheerder", "Manager", "Administrator"]
  // )){
  //   return <Redirect to='/agenda/overview'/>
  // }

  return (
    <AppBar className={classes.appBar + appBarClasses}>
      <Toolbar className={classes.container}>
        <div className={classes.flex}>
          {agendaLink}
          {dashboardLink}
        </div>
        <Hidden smDown={true} implementation="css">
          <AdminNavbarLinks />
        </Hidden>
        <Hidden mdUp={true} implementation="css">
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={props.handleDrawerToggle}
          >
            <Menu />
          </IconButton>
        </Hidden>
      </Toolbar>
    </AppBar>
  );
}

export default withHooksComponent(withStyles(headerStyle)(Header));
