import React, { Component } from 'react';

export class Conference extends Component {
    displayName = Conference.name

    constructor(props) {
        super(props);
        this.state = { 
            conferences: [], 
            loading: true,
            index: 0,
            count: 10
        };

        fetch('api/SampleData/Conferences?index=' + this.state.index + '&count=' + this.state.count)
        .then(response => response.json())
        .then(data => {
            this.setState({ conferences: data, loading: false });
        });
    }

    static renderConferencesTable(conferences) {
        return (
            <table className='table'>
                <thead>
                    <tr>
                        <th>会议编号</th>
                        <th>开始时间</th>
                        <th>结束时间</th>
                        <th>会场名</th>
                    </tr>
                </thead>
                <tbody>
                    {conferences.map(conference =>
                        <tr key={conference.conferenceId}>
                            <td>{conference.startTime}</td>
                            <td>{conference.endTime}</td>
                            <td>{conference.hallName}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Conference.renderConferencesTable(this.state.conferences);

        return (
            <div>
                <h1>Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }
}
