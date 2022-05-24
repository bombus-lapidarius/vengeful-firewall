~~~~~~
Amduat
~~~~~~

|badge0| |badge1|_ |badge2|_

  A transparent encryption layer for IPFS, implemented in F#


Project goal
============

Provide an easy to use symmetric encryption layer that can be used as a proxy
between client applications and a conventional IPFS node running on the local
machine (possibly on remote machines too in the future).


General concept
===============

Files (or blocks of binary data in general) handed over to the encryption layer
shall be encrypted on the fly. Only the encrypted version shall be stored via
the backing IPFS node.

Similarly, any requests for data from the backing IPFS network shall be silently
replaced with a request for the encrypted version of the corresponding data. The
encryption layer will then decrypt the content on the fly and return the
plaintext version instead.

To achieve this, a mapping between content ids for plaintext data and those for
the corresponding encrypted data must be stored in a secure way.

All configuration shall happen out-of-band, i.e. the software shall emulate a
conventional IPFS node to any client applications accessing it via RPC API. All
encryption-specific options shall be set via some well-known RPC API endpoints,
that do not usurp regular IPFS RPC API endpoints.


Status
======

The project is in a very early stage. The core functionality for the encryption
and the decryption of data blocks has been implemented. Currently, the only
supported cipher is aes-cbc-essiv:sha256 (using 128, 192 and 256 bit keys).

The next milestones are:

* implementing the dag-pb codec in F# as well as
* storing the mapping between content ids for plaintext data and those for
  the corresponding encrypted data using a HAMT,

followed by the replication of the minimal amount of IPFS functionality needed
to split files into blocks and to store encrypted versions of those blocks via
the underlying IPFS network.

All IPLD related code shall be written in a way that allows it to become the
nucleus of a complete .NET IPLD library in the future.


Licensing
=========

Dual-licensed under MIT and Apache-2.0, by way of the `Permissive License Stack
<https://protocol.ai/blog/announcing-the-permissive-license-stack/>`_.

Apache-2.0: https://www.apache.org/licenses/license-2.0

MIT: https://www.opensource.org/licenses/mit

.. |badge0| image:: https://gitlab.com/bombus-lapidarius/vengeful-firewall/badges/master/pipeline.svg
   :alt: pipeline status badge

.. |badge1| image:: https://img.shields.io/badge/License-Apache_2.0-blue.svg
   :alt: apache-2.0 license badge
.. _badge1: https://opensource.org/licenses/Apache-2.0

.. |badge2| image:: https://img.shields.io/badge/License-MIT-yellow.svg
   :alt: mit license badge
.. _badge2: https://opensource.org/licenses/MIT
