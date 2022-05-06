/* eslint-disable */
import React from "react";
import { withHooksComponent } from '../../custom-hooks/wrapperHook';
import withStyles from "@material-ui/core/styles/withStyles";
import visualisationStyle from "../../assets/jss/material-dashboard-react/components/visualisationStyle";
import { doGetApiCall, doJsonPostApiCall } from "../../utils/apicommunications";
import { Button } from "@material-ui/core";
import LineChart from "./chart";
import { First } from "react-bootstrap/lib/Pagination";
const ms = require('multiselect-react-dropdown');
const {Multiselect} = ms

interface Props {
  classes: any;
  location: any;
  userDetail: UserDetails;
  token: string;
}

interface State {
  selectedValues: 
  | {
    id: string;
    groupNumber: number;
    birthDate: Date;
    totalPigs: number;
    totalWeight: number;
    averageWeight: number;
  }[],

  groupsData:
  | {
    id: string;
    groupNumber: number;
    birthDate: Date;
    totalPigs: number;
    totalWeight: number;
    averageWeight: number;
  }[]
  | null,

  traceListGrowth: {
    x: number[],
    y: number[],
    type: string,
    name: string,
  }[],

  traceListVCGroups: {
    x: number[],
    y: number[],
    type: string,
    name: string
  }[];

  traceListFoodUseGroups: {
    x: number[],
    y: number[],
    type: string,
    name: string
  }[],

  done: boolean
}

class Visualisations extends React.Component<Props, State> {
  private userDetail: UserDetails;

  refs: any;
  constructor(props: Props) {
    super(props);
    this.state = {
      groupsData: [] = [],
      selectedValues: [] = [],
      traceListGrowth: [] = [],
      traceListVCGroups: [] = [],
      traceListFoodUseGroups: [] = [],
      done: false
    };
    this.userDetail = props.userDetail;

  }

onSelect(selectedList: any, selectedItem: any) {
     this.setState({selectedValues: selectedList});
     

}


onRemove(selectedList: any, removedItem: any) {
  this.setState({selectedValues: selectedList})
}

async getGroups(): Promise<void> {
  await doGetApiCall("PigGroup/Groups", this.userDetail.token, (data: any) => {
    this.setState({ groupsData: data });
  });
}

async getVisualizations(id: string, type: string, groupNumber: number): Promise<void> {
  await doGetApiCall("insights/" + type + '/' + id, this.userDetail.token, (data: {[key: string]: string}) => {
    let newTrace: {x: number[], y:number[], type: string, name: string} = {x: [], y:[], type:'scatter', name: "gr. " + groupNumber}
    let traceList = [... this.state.traceListGrowth]

    for (const [key, value] of Object.entries(data)) {
      newTrace.x.push(parseInt(key))
      newTrace.y.push(parseFloat(value))
     } 

     traceList.push(newTrace)
     this.setState({traceListGrowth: traceList})
  });


}

async getVisualizationVC(id: string, type: string, groupNumber: number): Promise<void> {
  await doGetApiCall("insights/" + type + '/' + id, this.userDetail.token, (data: {[key: string]: string}) => {
    let newTrace: {x: number[], y:number[], type: string, name: string} = {x: [], y:[], type:'scatter', name: "gr. " + groupNumber}
    let traceList = [... this.state.traceListVCGroups]

    for (const [key, value] of Object.entries(data)) {
      newTrace.x.push(parseInt(key))
      newTrace.y.push(parseFloat(value))
     } 

     traceList.push(newTrace)
     this.setState({traceListVCGroups: traceList})
  });


}

async getVisualizationFood(id: string, type: string, groupNumber: number): Promise<void> {
  await doGetApiCall("insights/" + type + '/' + id, this.userDetail.token, (data: {[key: string]: string}) => {
    let newTrace: {x: number[], y:number[], type: string, name: string} = {x: [], y:[], type:'scatter', name: "gr. " + groupNumber}
    let traceList = [... this.state.traceListFoodUseGroups]

    for (const [key, value] of Object.entries(data)) {
      newTrace.x.push(parseInt(key))
      newTrace.y.push(parseFloat(value))
     } 

     traceList.push(newTrace)
     this.setState({traceListFoodUseGroups: traceList})
  });


}

async visualize() {
  this.setState({traceListFoodUseGroups: []})
  this.setState({traceListGrowth: []})
  this.setState({traceListVCGroups: []})


  for(let i = 0; i < this.state.selectedValues.length; i++) {
    let group = this.state.selectedValues[i];
    this.getVisualizations(group.id, "GrowthGroups", group.groupNumber)
    this.getVisualizationFood(group.id, "FoodUseGroups", group.groupNumber)
    this.getVisualizationVC(group.id, "VCGroups", group.groupNumber)
  }

}

componentDidMount() {
  this.getGroups();
}

  render() {
    const { classes, ...rest } = this.props;
    let groupUis: JSX.Element[] = [];

    var layout = {
      title: 'Quarter 1 Growth',
      xaxis: {
        title: 'GDP per Capita',
        showgrid: false,
        zeroline: false
      },
      yaxis: {
        title: 'Percent',
        showline: false
      }
    };
    
    return (<div>
      <div className={classes.multiSelect}>
        <Multiselect 
          options={this.state.groupsData} // Options to display in the dropdown
          selectedValues={this.state.selectedValues} // Preselected value to persist in dropdown
          onSelect={(x: any, y: any) => this.onSelect(x, y)} // Function will trigger on select event
          onRemove={(x: any, y: any) => this.onRemove(x, y)} // Function will trigger on remove event
          displayValue="groupNumber" // Property name to display in the dropdown options
          closeOnSelect={false}
          showCheckbox={true}
        />
      </div>
        <Button variant="outlined" color="primary" className={classes.searchButton} onClick={() => this.visualize()}>Visualiseer</Button>
        <LineChart yaxis={{title: "voeder consumptie per kg"}} xaxis={{title: "Leeftijd in dagen"}}data={this.state.traceListVCGroups}  name="Voeder conversie (kg_voer/kg_vlees)"></LineChart>
        <LineChart yaxis={{title: "groei in kg per dag"}} xaxis={{title: "Leeftijd in dagen"}} data={this.state.traceListGrowth}  name="Groei (kg/dag)"></LineChart>
        <LineChart yaxis={{title: "voeder consumptie in kg per dag"}} xaxis={{title: "Leeftijd in dagen"}}data={this.state.traceListFoodUseGroups}  name="Voeder consumptie (kg/dag)"></LineChart>
      </div>)
  }
}

export default withHooksComponent(withStyles(visualisationStyle)(Visualisations));
