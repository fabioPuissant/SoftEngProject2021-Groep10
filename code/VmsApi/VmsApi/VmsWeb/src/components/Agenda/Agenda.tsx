import React from "react";
// react plugin for creating charts
import ChartistGraph from "react-chartist";
// @material-ui/core
import withStyles from "@material-ui/core/styles/withStyles";

import dashboardStyle from "../../assets/jss/material-dashboard-react/views/dashboardStyle";
import { MyMonthlyCalendar } from "./calendar";
import { useMonthlyCalendar } from "@zach.codes/react-calendar";
import { withHooksComponent } from "../../custom-hooks/wrapperHook";

interface Props {
  classes: any;
  userDetail: UserDetails;
  token: string;
}

interface State {
  value: number;
}

class Agenda extends React.Component<Props, State> {
  private userDetail: UserDetails;

  constructor(props: Props) {
    super(props);
    this.state = {
      value: 0,
    };
    this.userDetail = props.userDetail;
    // Todo Dave ==> via userDetail kunt ge token en rollen opvragen 
    //           this.userDetail.roles geeft de rollen van de huidige user!!
    //           hiermee kan je conditioneel renderen if (x in this.userDetail.roles...)
  }

  //https://reactjsexample.com/simple-and-flexible-events-calendar-written-in-react/
  render() {
    const { classes } = this.props;
    return (
      <div>
        <MyMonthlyCalendar />
      </div>
    );
  }
}

export default withHooksComponent(withStyles(dashboardStyle)(Agenda));
