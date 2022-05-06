import React from "react";
// @material-ui/core
import withStyles from "@material-ui/core/styles/withStyles";
import Icon from "@material-ui/core/Icon";

// core components
import GridItem from "../../components/Grid/GridItem";
import GridContainer from "../../components/Grid/GridContainer";
import Card from "../../components/Card/Card";
import CardHeader from "../../components/Card/CardHeader";
import CardIcon from "../../components/Card/CardIcon";

import dashboardStyle from "../../assets/jss/material-dashboard-react/views/dashboardStyle";
import { Link, Redirect } from "react-router-dom";
import { withHooksComponent } from "../../custom-hooks/wrapperHook";
import { checkRole } from "../../utils/RoleChecker";

interface Props {
  classes: any;
  userDetail: UserDetails;
  token: string;
}

interface State {
  value: number;
}

class Dashboard extends React.Component<Props, State> {
  private userDetail: any;
  constructor(props: any) {
    super(props);
    this.state = {
      value: 0,
    };
    this.handleChange = this.handleChange.bind(this);
    this.handleChangeIndex = this.handleChangeIndex.bind(this);
    this.userDetail = props.userDetail;
  }

  handleChange = (event: any, value: number) => {
    this.setState({ value });
  };

  handleChangeIndex = (index: number) => {
    this.setState({ value: index });
  };

  render() {
    const { classes } = this.props;

    let dashboardOptions = [];
    let dashboardLinks = []; 

    if (checkRole(this.props.userDetail.roles != undefined ? this.props.userDetail.roles : [], ["Manager", "Weger", "Voedingsbeheerder"])){
      dashboardOptions.push(<GridItem xs={12} sm={6} md={4}>
        <Link to={"/dashboard/groups"} className={classes.title}>
          <Card>
            <CardHeader color="warning" stats={true} icon={true}>
              <CardIcon color="warning">
                <Icon>content_copy</Icon>
              </CardIcon>
              <h3 className={classes.cardTitle}>Groepen</h3>
            </CardHeader>
          </Card>
        </Link>
      </GridItem>)
      dashboardLinks.push("/dashboard/groups");
    };

    if (checkRole(this.props.userDetail.roles != undefined ? this.props.userDetail.roles : [], [])){
      dashboardOptions.push(<GridItem xs={12} sm={6} md={4}>
        <Link to={"/dashboard/visualisations"} className={classes.title}>
          <Card>
            <CardHeader color="warning" stats={true} icon={true}>
              <CardIcon color="warning">
                <Icon>content_copy</Icon>
              </CardIcon>
              <h3 className={classes.cardTitle}>Visualisatie</h3>
            </CardHeader>
          </Card>
        </Link>
      </GridItem>);
      dashboardLinks.push("/dashboard/visualisations");
    };

    if(checkRole(this.props.userDetail.roles != undefined ? this.props.userDetail.roles : [], [])){
      dashboardOptions.push(<GridItem xs={12} sm={6} md={4}>
        <Link to={"/dashboard/users"} className={classes.title}>
          <Card>
            <CardHeader color="warning" stats={true} icon={true}>
              <CardIcon color="warning">
                <Icon>content_copy</Icon>
              </CardIcon>
              <h3 className={classes.cardTitle}>Werknemers</h3>
            </CardHeader>
          </Card>
        </Link>
      </GridItem>);
      dashboardLinks.push("/dashboard/users");
    };

    let result: JSX.Element;
    if (dashboardOptions.length === 0){
      result = (<Redirect to="/agenda/overview"/>);
    }
    else if (dashboardOptions.length === 1) {
      result = (<div>
        <GridContainer>
          <Redirect to={dashboardLinks[0]}/>
        </GridContainer>
      </div>);
    }
    else{
      result = (<div>
        <GridContainer>
          {dashboardOptions}
        </GridContainer>
      </div>);
    }

    return (
      result
    );
  }
}

export default withHooksComponent(withStyles(dashboardStyle)(Dashboard));
