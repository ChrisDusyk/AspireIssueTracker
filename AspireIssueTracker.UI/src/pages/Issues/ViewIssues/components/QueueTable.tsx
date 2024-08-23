import {
  Table,
  TableContainer,
  Tbody,
  Td,
  Text,
  Th,
  Thead,
  Tr,
  Link as ChakraLink,
} from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";
import moment from "moment";
import { Issue } from "../../../../types";

export type QueueTableProps = {
  data: Issue[];
  queueName: string;
};

export default function QueueTable({ data, queueName }: QueueTableProps) {
  return (
    <div>
      <Text fontSize="3xl">{queueName}</Text>
      <TableContainer>
        <Table variant="simple">
          <Thead>
            <Tr>
              <Th>Id</Th>
              <Th>Title</Th>
              <Th>Reported By</Th>
              <Th>Created At</Th>
              <Th></Th>
            </Tr>
          </Thead>
          <Tbody>
            {data?.map((issue) => (
              <Tr key={issue.id}>
                <Td>
                  <ChakraLink as={RouterLink} to={`/issues/${issue.id}`}>
                    {issue.id}
                  </ChakraLink>
                </Td>
                <Td>{issue.title}</Td>
                <Td>{issue.reportedBy?.name ?? "N/A"}</Td>
                <Td>{moment(issue.createdAt).format("dddd")}</Td>
                <Td>View, Claim</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </TableContainer>
    </div>
  );
}
