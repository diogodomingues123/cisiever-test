{{- if . }}
{{- range . }}
<h3>Target <code>{{ escapeXML .Target }}</code></h3>
{{- if (and (eq (len .Vulnerabilities) 0) (eq (len .Misconfigurations) 0) (eq (len .Secrets) 0)) }}
<h4>Nothing found</h4>
{{- else }} 
{{- if (gt (len .Vulnerabilities) 0) }}
<h4>Vulnerabilities ({{ len .Vulnerabilities }})</h4>
<table>
    <tr>
        <th>Package</th>
        <th>ID</th>
        <th>Severity</th>
        <th>Installed Version</th>
        <th>Fixed Version</th>
    </tr>
    {{- range .Vulnerabilities }}
    <tr>
        <td><code>{{ escapeXML .PkgName }}</code></td>
        <td>{{ escapeXML .VulnerabilityID }}</td>
        <td>{{ escapeXML .Severity }}</td>
        <td>{{ escapeXML .InstalledVersion }}</td>
        <td>{{ escapeXML .FixedVersion }}</td>
    </tr>
    {{- end }}
</table>
{{- end }}
{{- if (gt (len .Misconfigurations ) 0) }}
<h4>Misconfigurations ({{ len .Misconfigurations }})</h4>
<table>
    <tr>
        <th>Type</th>
        <th>ID</th>
        <th>Check</th>
        <th>Severity</th>
        <th>Message</th>
    </tr>
    {{- range .Misconfigurations }}
    <tr>
        <td>{{ escapeXML .Type }}</td>
        <td>{{ escapeXML .ID }}</td>
        <td>{{ escapeXML .Title }}</td>
        <td>{{ escapeXML .Severity }}</td>
        <td>
          {{ escapeXML .Message }}
          <br><a href={{ escapeXML .PrimaryURL | printf "%q" }}>{{ escapeXML .PrimaryURL }}</a></br>
        </td>
    </tr>
    {{- end }}
</table>
{{- end }}
{{- if (gt (len .Secrets ) 0) }}
<h4>Secrets ({{ len .Secrets }})</h4>
<table>
    <tr>
        <th>Type</th>
        <th>ID</th>
        <th>Severity</th>
        <th>Lines</th>
        <th>Match</th>
    </tr>
    {{- range .Secrets }}
    <tr>
        <td>{{ escapeXML (toString .Category) }}</td>
        <td>{{ escapeXML .RuleID }}</td>
        <td>{{ escapeXML .Severity }}</td>
        <td>{{ escapeXML (toString .StartLine) }}-{{ escapeXML (toString .EndLine) }}</td>
        <td>{{ escapeXML .Match }}</td>
    </tr>
    {{- end }}
</table>
{{- end }}
{{- end }}
{{- end }}
{{- else }}
<h3>Trivy Returned Empty Report</h3>
{{- end }}