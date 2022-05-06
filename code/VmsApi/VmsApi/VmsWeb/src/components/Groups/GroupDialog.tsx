/* eslint-disable */
import React from "react";
import "perfect-scrollbar/css/perfect-scrollbar.css";
// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";

import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
// core components
import tableStyle from "../../assets/jss/material-dashboard-react/components/tableStyle";

import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import { Col, Container, Row } from "react-bootstrap";
import GroupFoodTable from "./GroupFoodTable";
import GroupWeightTable from "./GroupWeightTable";
import GroupInfoTable from "./GroupInfoTable";
import { doGetApiCall, doJsonDeleteApiCall, doJsonPutApiCall } from "../../utils/apicommunications";
import { measureMemory } from "node:vm";

// core components

interface Props {
  classes: any;
  groupId: string;
  userDetail: UserDetails;
  onClose: () => void;
}

interface State {
  group: {
    id: string;
    groupNumber: number;
    birthDate: Date;
    currentAmount: number;
    currentTotalWeight: number;
    currentIndividualWeight: number;
  };
  measurePoints: {
    id: string;
    date: Date;
    weight: number;
    amount: number;
    changed: boolean;
    deleted: boolean;
  }[];
  foodPurchases: {
    id: string;
    date: Date;
    amount: number;
    changed: boolean;
    deleted: boolean;
  }[];
}

class GroupDialog extends React.Component<Props, State> {
  refs: any;
  constructor(props: Props) {
    super(props);
    this.state = {
      group: {
        id: "0000",
        groupNumber: 0,
        birthDate: new Date("24/02/2020"),
        currentAmount: 19,
        currentTotalWeight: 500,
        currentIndividualWeight: 26.31,
      },
      measurePoints: [],
      foodPurchases: [],
    };
  }

  handleSave = async() => {
    let foodUpdates = this.state.foodPurchases.filter((x) => x.changed);
    let measureUpdates = this.state.measurePoints.filter((x) => x.changed);
    let foodDeletes = this.state.foodPurchases.filter((x) => x.deleted);
    let measureDeletes = this.state.measurePoints.filter((x) => x.deleted);

    for(let i=0; i<foodUpdates.length; i++){
      await doJsonPutApiCall("FoodPurchase/purchases/" + foodUpdates[i].id, this.props.userDetail.token, foodUpdates[i], (data: {}) => {});
    }
    for(let i=0; i<measureUpdates.length; i++){
      await doJsonPutApiCall("MeasurePoint/points/" + measureUpdates[i].id, this.props.userDetail.token, measureUpdates[i], (data: {}) => {});
    }

    for(let i=0; i<foodDeletes.length; i++){
      await doJsonDeleteApiCall("FoodPurchase/purchases/" + foodDeletes[i].id, this.props.userDetail.token, (data: {}) => {});
    }
    for(let i=0; i<measureDeletes.length; i++){
      await doJsonDeleteApiCall("MeasurePoint/points/" + measureDeletes[i].id, this.props.userDetail.token, (data: {}) => {});
    }
    this.props.onClose();
  };

  async getGroup(): Promise<void> {
    if (this.props.groupId !== null) {
      await doGetApiCall(
        "PigGroup/Groups/" + this.props.groupId, this.props.userDetail.token,
        (data: {
          id: string;
          birthDate: string;
          groupNumber: string;
          measurePoints: {
            amount: number;
            date: Date;
            id: string;
            weight: number;
          }[];
          foodPurchases: {
            amount: number;
            date: Date;
            id: string;
          }[];
        }) => {
          let latestMeasurePoint = data.measurePoints.sort(
            (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()
          )[0];
          this.setState({
            group: {
              id: data.id,
              birthDate: new Date(data.birthDate),
              groupNumber: Number(data.groupNumber),
              currentAmount: latestMeasurePoint.amount,
              currentTotalWeight: latestMeasurePoint.weight,
              currentIndividualWeight:
                latestMeasurePoint.weight / latestMeasurePoint.amount,
            },
            measurePoints: data.measurePoints.map((x) => {return {...x, changed: false, deleted: false} }),
            foodPurchases: data.foodPurchases.map((x) => {return {...x, changed: false, deleted: false}}),
          });
        }
      );
    }
  }

  handleClose = () => {
    this.props.onClose();
  };

  componentDidMount() {
    this.getGroup();
  }

  render() {
    const { classes, ...rest } = this.props;

    return (
      <Dialog
        open={this.props.groupId ? true : false}
        onClose={this.handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">
          {"Groep: " + this.state.group.groupNumber}
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            <GroupInfoTable groupInfo={this.state.group} />

            {this.state.measurePoints.length !== 0? <GroupWeightTable
              measurePoints={this.state.measurePoints}
              birthDate={this.state.group.birthDate}
              onChange={(x: any) => {
                this.setState({ measurePoints: x });
              }}
            />: null}
            {this.state.foodPurchases.length !== 0? <GroupFoodTable
              birthDate={this.state.group.birthDate}
              foodPurchases={this.state.foodPurchases}
              onChange={(x: any) => {
                this.setState({ foodPurchases: x });
              }}
            />: null}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button color="primary" onClick={this.handleClose}>
            Annuleren
          </Button>
          <Button color="primary" autoFocus onClick={this.handleSave}>
            Opslaan
          </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default withStyles(tableStyle)(GroupDialog);
