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
import moment from "moment";

// core components

interface Props {
  classes: any;
  groupInfo: {
    id: string;
    groupNumber: number;
    birthDate: Date;
    currentAmount: number;
    currentTotalWeight: number;
    currentIndividualWeight: number;
  };
}

interface State {}

class GroupInfoTable extends React.Component<Props, State> {

  render() {
    const { classes, ...rest } = this.props;
    let groupUis: JSX.Element[] = [];
    const tableHeaderColor = "primary";
    const tableHead = [
        "Geboorte",
        "# Biggen",
        "Totaal gewicht (kg)",
        "Gemiddeld gewicht/big (kg)",
      ];

    return (
        <Table className={classes.table}>
        <TableHead
          className={classes[tableHeaderColor + "TableHeader"]}
        >
          <TableRow>
            {tableHead.map((prop: any, key: any) => {
              return (
                <TableCell
                  className={
                    classes.tableCell + " " + classes.tableHeadCell
                  }
                  key={key}
                >
                  {prop}
                </TableCell>
              );
            })}
          </TableRow>
        </TableHead>
        <TableBody>
          <TableRow>
            <TableCell className={classes.tableCell}>
              {moment(this.props.groupInfo.birthDate).format('DD/MM/YYYY')}
            </TableCell>
            <TableCell className={classes.tableCell}>
              {this.props.groupInfo.currentAmount}
            </TableCell>
            <TableCell className={classes.tableCell}>
              {this.props.groupInfo.currentTotalWeight}
            </TableCell>
            <TableCell className={classes.tableCell}>
              {this.props.groupInfo.currentIndividualWeight.toFixed(2)}
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    );
  }
}

export default withStyles(tableStyle)(GroupInfoTable);
