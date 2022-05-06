import React from 'react';
import classNames from 'classnames';
// @material-ui/core components
import withStyles from '@material-ui/core/styles/withStyles';
import MenuItem from '@material-ui/core/MenuItem';
import MenuList from '@material-ui/core/MenuList';
import Grow from '@material-ui/core/Grow';
import Paper from '@material-ui/core/Paper';
import ClickAwayListener from '@material-ui/core/ClickAwayListener';
import Hidden from '@material-ui/core/Hidden';
import Poppers from '@material-ui/core/Popper';
// @material-ui/icons
import Person from '@material-ui/icons/Person';
import Notifications from '@material-ui/icons/Notifications';
// core components
import Button from '../CustomButtons/Button';

import headerLinksStyle from '../../assets/jss/material-dashboard-react/components/headerLinksStyle';
import { withHooksComponent } from '../../custom-hooks/wrapperHook';
import { Link } from 'react-router-dom';

interface Props {
  classes: any;
  userDetail: UserDetails;
  token: string;
  logout: Function;
}

class HeaderLinks extends React.Component<Props, {}> {
  anchorEl: any;

  state = {
    open: false
  };

  handleToggle = () => {
    this.setState({ open: !this.state.open });
  }

  handleClose = (event: any) => {
    if (this.anchorEl.contains(event.target)) {
      return;
    }

    this.setState({ open: false });
  }

  render() {
    const { classes, userDetail, logout } = this.props;
    const { open } = this.state;

    return (
      <div>

        <div className={classes.manager}>
          <Button
            buttonRef={(node: any) => {
              this.anchorEl = node;
            }}
            color={window.innerWidth > 959 ? 'transparent' : 'white'}
            justIcon={window.innerWidth > 959}
            simple={!(window.innerWidth > 959)}
            aria-owns={open ? 'menu-list-grow' : null}
            aria-haspopup="true"
            aria-label="Person"
            onClick={this.handleToggle}
            className={classes.buttonLink}
          >
            {/*
            <Notifications className={classes.icons} />
            <span className={classes.notifications}>5</span>
            */}
            <Person className={classes.icons} />
            <Hidden mdUp={true} implementation="css">
              <p className={classes.icons}>
                Profiel
              </p>
            </Hidden>
          </Button>
          <Poppers
            open={open}
            anchorEl={this.anchorEl}
            transition={true}
            disablePortal={true}
            className={
              classNames({ [classes.popperClose]: !open }) +
              ' ' +
              classes.pooperNav
            }
          >
            {({ TransitionProps, placement }) => (
              <Grow
                {...TransitionProps}
                // id="menu-list-grow"
                style={{
                  transformOrigin:
                    placement === 'bottom' ? 'center top' : 'center bottom'
                }}
              >
                <Paper>
                  <ClickAwayListener onClickAway={this.handleClose}>
                    <MenuList role="menu">
                      <MenuItem disabled={true}>
                        {userDetail.firstname + " " + userDetail.lastname}
                      </MenuItem>
                      <Link to={'/dashboard/user'} className={classes.title}>
                        <MenuItem
                          onClick={this.handleClose}
                          className={classes.dropdownItem}
                        >
                          Naar profiel
                        </MenuItem>
                      </Link>
                      <MenuItem
                        onClick={(e: any) => { this.handleClose(e); logout(); }}
                        className={classes.dropdownItem}
                      >
                        Logout
                      </MenuItem>
                    </MenuList>
                  </ClickAwayListener>
                </Paper>
              </Grow>
            )}
          </Poppers>
        </div>

        {/* 
  <Button
          color={window.innerWidth > 959 ? 'transparent' : 'white'}
          justIcon={window.innerWidth > 959}
          simple={!(window.innerWidth > 959)}
          aria-label="Person"
          className={classes.buttonLink}
        >
          <Person className={classes.icons} />
          <Hidden mdUp={true} implementation="css">
            <p className={classes.linkText}>Profile</p>
          </Hidden>
        </Button>
*/}

      </div>
    );
  }
}

export default withHooksComponent(withStyles(headerLinksStyle)(HeaderLinks));
