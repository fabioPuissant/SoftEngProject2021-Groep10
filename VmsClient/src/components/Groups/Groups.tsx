/* eslint-disable */
import React from "react";
import { withHooksComponent } from '../../custom-hooks/wrapperHook';

import { Switch, Route, Redirect } from "react-router-dom";
// creates a beautiful scrollbar
import PerfectScrollbar from "perfect-scrollbar";
import "perfect-scrollbar/css/perfect-scrollbar.css";
// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";
import groupsStyle from "../../assets/jss/material-dashboard-react/components/groupsStyle";
import { Card, Fab, Tooltip } from "@material-ui/core";
import GridContainer from "../Grid/GridContainer";
import GridItem from "../Grid/GridItem";

import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";

import AddIcon from "@material-ui/icons/Add";
// core components
import tableStyle from "../../assets/jss/material-dashboard-react/components/tableStyle";

import Popup from "reactjs-popup";
import CardBody from "../Card/CardBody";
import { Col, Container, Row } from "react-bootstrap";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import { doGetApiCall } from "../../utils/apicommunications";
import GroupDialog from "./GroupDialog";
import AddGroupDialog from "./AddGroupDialog";
import moment from "moment";
import { checkRole } from "../../utils/RoleChecker";
import AddWeightDialog from "./AddWeightDialog";
import AddFoodDialog from "./AddFoodDialog";

// core components

interface Props {
  classes: any;
  location: any;
  userDetail: UserDetails;
  token: string;
}

interface State {
  currentGroup: string | null;
  addGroup: boolean;
  addWeight: boolean;
  addFood: boolean;
  groupsData:
  | {
    id: string;
    groupNumber: number;
    birthDate: Date;
    totalPigs: number;
    totalWeight: number;
    averageWeight: number;
  }[]
  | null;
}

class Groups extends React.Component<Props, State> {
  private userDetail: UserDetails;

  refs: any;
  constructor(props: Props) {
    super(props);
    this.state = {
      currentGroup: null,
      addGroup: false,
      addWeight: false,
      addFood: false,
      groupsData: [
        
      ],

    };
    this.userDetail = props.userDetail;

  }

  openDialog(): boolean {
    return this.state.currentGroup ? true : false;
  }

  async getGroups(): Promise<void> {
    await doGetApiCall("PigGroup/Groups", this.userDetail.token, (data: any) => {
      this.setState({ groupsData: data });
    });
  }

  handleClose = () => {
    this.setState({ currentGroup: null });
  };

  componentDidMount() {
    this.getGroups();
  }

  addGroup = async (
    id: string,
    groupNumber: number,
    birthDate: string,
    amount: number,
    totalWeight: number,
    averageWeight: number,
    foodWeight: number,
  ) => {
    let hulp = this.state.groupsData ? this.state.groupsData : [];
    hulp.push({
      id,
      groupNumber,
      birthDate: new Date(birthDate),
      totalPigs: amount,
      totalWeight: totalWeight,
      averageWeight: averageWeight,
    });
    this.setState({ groupsData: hulp });
  };

  addWeight = async (
    id: string,
    groupNumber: number,
    date: string,
    amount: number,
    totalWeight: number,
    averageWeight: number
  ) => {
    let hulp = this.state.groupsData ? this.state.groupsData : [];
    hulp.forEach(group => {
      if(group.groupNumber === groupNumber) {   //TODO check of datum later is dan laatste meeting (geef de laatste meeting mee wanneer een group wordt opgehaald)
        group.totalPigs = amount;
        group.totalWeight = totalWeight;
        group.averageWeight = averageWeight;
      }
    })

    this.setState({ groupsData: hulp });
  };

  customTable() {
    const groupDialog = this.state.currentGroup ? (
      <GroupDialog
        groupId={this.state.currentGroup}
        onClose={() => {
          this.setState({ currentGroup: null });
          this.getGroups();
        }}
        userDetail={this.userDetail}
      />
    ) : (
      <span />
    );

    const tableHeaderColor = "primary";
    const tableHead = [
      "Groep Nr.",
      "Geboorte",
      "# Biggen",
      "Totaal gewicht (kg)",
      "Gemiddeld gewicht/big (kg)",
    ];
    const { classes } = this.props;
    const addButton = checkRole(this.userDetail.roles != undefined ? this.userDetail.roles : [], ['Manager']) ? <Button variant="outlined" color="primary" onClick={() => this.setState({ addGroup: true })}>Groep toevoegen</Button> : <div></div> 
    const addWeight = checkRole(this.userDetail.roles != undefined ? this.userDetail.roles : [], ['Manager', 'Weger']) ? <Button variant="outlined" color="primary" onClick={() => this.setState({ addWeight: true })}>Gewicht toevoegen</Button> : <div></div> 
    const addFood = checkRole(this.userDetail.roles != undefined ? this.userDetail.roles : [], ['Manager', 'Voedingsbeheerder']) ? <Button variant="outlined" color="primary" onClick={() => this.setState({ addFood: true })}>Voeder toevoegen</Button> : <div></div> 

    return (
      <span className={classes.tableResponsive}>

       {addButton}
       &nbsp;&nbsp;&nbsp;
       {addWeight}
       &nbsp;&nbsp;&nbsp;
       {addFood}
       
        <Table className={classes.table}>
          <TableHead className={classes[tableHeaderColor + "TableHeader"]}>
            <TableRow>
              {tableHead.map((prop: any, key: any) => {
                return (
                  <TableCell
                    className={classes.tableCell + " " + classes.tableHeadCell}
                    key={key}
                  >
                    {prop}
                  </TableCell>
                );
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {this.state.groupsData!.map((prop, key) => {
              return (
                <TableRow
                  key={key}
                  onClick={() => {
                    this.setState({ currentGroup: prop.id });
                  }}
                >
                  <TableCell className={classes.tableCell}>
                    {prop.groupNumber}
                  </TableCell>
                  <TableCell className={classes.tableCell}>
                    {moment(prop.birthDate).format("DD/MM/YYYY")}
                  </TableCell>
                  <TableCell className={classes.tableCell}>
                    {prop.totalPigs}
                    {/* {20} */}
                  </TableCell>
                  <TableCell className={classes.tableCell}>
                    {prop.totalWeight}
                    {/* {500} */}
                  </TableCell>
                  <TableCell className={classes.tableCell}>
                    {prop.averageWeight.toFixed(2)}
                    {/* {25} */}
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
        {groupDialog}


        <AddGroupDialog
          open={this.state.addGroup}
          {...this.props}
          onClose={() => {this.setState({ addGroup: false }); this.getGroups()}}
          groupAdd={this.addGroup}
        />
        
        <AddWeightDialog
          open={this.state.addWeight}
          {...this.props}
          onClose={() => {this.setState({ addWeight: false }); this.getGroups()}}
          weightAdd={this.addWeight}
        />

        <AddFoodDialog
          open={this.state.addFood}
          {...this.props}
          onClose={() => {this.setState({ addFood: false }); this.getGroups()}}
        />

      </span>
    );
  }

  render() {
    const { classes, ...rest } = this.props;
    let groupUis: JSX.Element[] = [];
    
    return this.customTable();
  }
}

export default withHooksComponent(withStyles(tableStyle)(Groups));
