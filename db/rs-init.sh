#!/bin/bash

mongosh <<EOF
var config = {
    "_id": "reports-rs",
    "version": 1,
    "members": [
        {
            "_id": 1,
            "host": "reports-db:27017",
            "priority": 1
        }
    ]
};
rs.initiate(config, { force: true });
rs.status();
EOF